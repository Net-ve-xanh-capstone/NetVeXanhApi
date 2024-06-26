using System.Runtime.InteropServices.JavaScript;
using Application.BaseModels;
using Application.IService;
using Application.SendModels.Painting;
using Application.SendModels.Schedule;
using Application.ViewModels.ScheduleViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infracstructures;

namespace Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Create

    public async Task<bool> CreateScheduleForPreliminaryRound(ScheduleRequest schedule)
    {
        //Get Paintings Of Preliminary round
        var listPainting = await _unitOfWork.PaintingRepo.ListPaintingForPreliminaryRound(schedule.RoundId);

        var award = _unitOfWork.RoundRepo.GetRoundDetail(schedule.RoundId).Result?.EducationalLevel.Award
            .FirstOrDefault(a => a.Rank == RankAward.Preliminary.ToString());
        
        if (award == null)
        {
            throw new Exception("Award not found.");
        }

        int quantityAward = award.Quantity;
        
        List<List<Painting>> result = SplitList(listPainting, schedule.ListExaminer.Count);
        
        //Create Schedule by number of Examiner

        for(int i = 0; i < schedule.ListExaminer.Count; i++)
        {
            var newSchedule = new Schedule();
            newSchedule.Id = Guid.NewGuid();
            newSchedule.ExaminerId = schedule.ListExaminer[i];
            newSchedule.RoundId = schedule.RoundId;
            newSchedule.Description = schedule.Description;
            newSchedule.Status = ScheduleStatus.Active.ToString();
            
            //Add award schudele
            var newAwardSchedule = new AwardSchedule();
            newAwardSchedule.Id = Guid.NewGuid();
            newAwardSchedule.ScheduleId = newSchedule.Id;
            newAwardSchedule.AwardId = award.Id;
            
            if (i == schedule.ListExaminer.Count - 1)
            {
                newAwardSchedule.Quantity = quantityAward;
            }
            else
            {
                newAwardSchedule.Quantity = (int)Math.Ceiling(award.Quantity / (double)schedule.ListExaminer.Count);
                quantityAward -= newAwardSchedule.Quantity;
            }

            newSchedule.AwardSchedule = new List<AwardSchedule>();
            newSchedule.AwardSchedule.Add(newAwardSchedule);
            
            await _unitOfWork.ScheduleRepo.AddAsync(newSchedule);
            
            //Change ScheduleID in Paiting
            result[i].ForEach(item => item.ScheduleId = newSchedule.Id);
        }
        return await _unitOfWork.SaveChangesAsync()>0;
        
    }

    public async Task<bool> CreateScheduleForFinalRound(ScheduleRequest schedule)
    {
        try
        {
            //Get Paintings Of Preliminary round
            var listPainting = await _unitOfWork.PaintingRepo.ListPaintingForFinalRound(schedule.RoundId);
            List<List<Painting>> result = SplitList(listPainting, schedule.ListExaminer.Count);

        
            //Get all award of educationLevel
            var award = _unitOfWork.RoundRepo.GetRoundDetail(schedule.RoundId).Result?.EducationalLevel.Award.Where(a => a.Rank != RankAward.Preliminary.ToString()).OrderBy(a => (RankAward)Enum.Parse(typeof(RankAward), a.Rank)).ToList();
            if (award == null)
            {
                throw new Exception("Award not found.");
            }
            var listQuantity = award.OrderBy(a => (RankAward)Enum.Parse(typeof(RankAward), a.Rank)).Select(a => a.Quantity).ToList();
            
            //Create Schedule 

            var listSchedule = new List<Schedule>();
            for (int i = 0; i < schedule.ListExaminer.Count; i++)
            {
                var newSchedule = new Schedule();
                newSchedule.Id = Guid.NewGuid();
                newSchedule.ExaminerId = schedule.ListExaminer[i];
                newSchedule.RoundId = schedule.RoundId;
                newSchedule.Description = schedule.Description;
                newSchedule.Status = ScheduleStatus.Active.ToString();
            
                //Create AwardSchedule
                var listAwardSchedule = new List<AwardSchedule>();
                for (int j = 0; j < award.Count; j++)
                {
                    if (listQuantity[j] == 0)
                    {
                        continue;
                    }
                    var newAwardSchedule = new AwardSchedule();
                    //In this case, the quantity of award just have 1
                    if (listQuantity[j] == 1)
                    {
                        newAwardSchedule.ScheduleId = newSchedule.Id;
                        newAwardSchedule.AwardId = award[j].Id;
                        newAwardSchedule.Quantity = listQuantity[j];
                        listQuantity[j] = 0;
                        listAwardSchedule.Add(newAwardSchedule);
                    }
                    else
                    {
                        newAwardSchedule.ScheduleId = newSchedule.Id;
                        newAwardSchedule.AwardId = award[j].Id;
                    
                        //In this case, this is the last loop
                        if (i == schedule.ListExaminer.Count - 1)
                        {
                            newAwardSchedule.Quantity = listQuantity[j];
                        }
                        else
                        {
                            newAwardSchedule.Quantity = (int)Math.Ceiling(award[j].Quantity / (double)schedule.ListExaminer.Count);
                            listQuantity[j] -= newAwardSchedule.Quantity;
                        }
                        listAwardSchedule.Add(newAwardSchedule);
                    }
                    
                }

                newSchedule.AwardSchedule = new List<AwardSchedule>();
                newSchedule.AwardSchedule = listAwardSchedule;
                
                //Change ScheduleID in Paiting
                result[i].ForEach(item => item.ScheduleId = newSchedule.Id);
                
                //Add to list
                listSchedule.Add(newSchedule);
            }

            listSchedule.Count();
            await _unitOfWork.ScheduleRepo.AddRangeAsync(listSchedule);
            return await _unitOfWork.SaveChangesAsync()>0;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    public List<List<Painting>> SplitList(List<Painting> list, int n)
    {
        var result = new List<List<Painting>>();
        int chunkSize = (int)Math.Ceiling(list.Count / (double)n);

        for (int i = 0; i < n; i++)
        {
            var chunk = list.Skip(i * chunkSize).Take(chunkSize).ToList();
            if (chunk.Any()) // Nếu chunk có phần tử thì mới thêm vào result
            {
                result.Add(chunk);
            }
        }
        return result;
    }

    #endregion

    #region Get All

    public async Task<(List<ScheduleViewModel>, int)> GetListSchedule(ListModels listModels)
    {
        var list = await _unitOfWork.ScheduleRepo.GetAllAsync();
        list = (List<Schedule>)list.Where(x => x.Status == "ACTIVE");

        var result = new List<Schedule>();

        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<ScheduleViewModel>>(result), totalPages);
    }

    #endregion

    #region Get By Id

    public async Task<ScheduleViewModel?> GetScheduleById(Guid id)
    {
        var Schedule = await _unitOfWork.ScheduleRepo.GetById(id);
        if (Schedule == null) throw new Exception("Khong tim thay Schedule");
        return _mapper.Map<ScheduleViewModel>(Schedule);
    }

    #endregion

    #region Rating

    public async Task<bool> RatingPreliminaryRound(RatingPreliminaryRound ratingPainting)
    {
        var schedules = await _unitOfWork.ScheduleRepo.GetById(ratingPainting.ScheduleId);
        if (schedules.Painting.Any(p => p.Status != PaintingStatus.Accepted.ToString()))
        {
            return false;
        }

        if (ratingPainting.Paintings.Except(schedules.Painting.Select(p => p.Id)).ToList().Any()){
            throw new Exception("Have ID not Exist In schedule");
        }

        ratingPainting.Paintings.Except(schedules.Painting.Select(p => p.Id)).ToList();
        
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Contains(p.Id)).ToList();
        var listNotPass = schedules.Painting.Where(p => !ratingPainting.Paintings.Contains(p.Id)).ToList();
        
        listPass.ForEach(p => p.Status = PaintingStatus.Pass.ToString());
        listNotPass.ForEach(p => p.Status = PaintingStatus.NotPass.ToString());
        schedules.Painting.ToList().ForEach(p => p.FinalDecisionTimestamp = DateTime.Now);
        if (listPass.Count != schedules.AwardSchedule.First().Quantity)
        {
            throw new Exception("The Quantity of paiting is wrong");
        }
        
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RatingFinalRound(RatingFinalRound ratingPainting)
    {
        var schedules = await _unitOfWork.ScheduleRepo.GetById(ratingPainting.ScheduleId);
        if (schedules.Painting.Any(p => p.Status != PaintingStatus.FinalRound.ToString()))
        {
            return false;
        }

        if (ratingPainting.First != null && ratingPainting.First.Except(schedules.Painting.Select(p => p.Id)).ToList().Any()){
            throw new Exception("Have ID not Exist In schedule");
        }
        if (ratingPainting.Second != null && ratingPainting.Second.Except(schedules.Painting.Select(p => p.Id)).ToList().Any()){
            throw new Exception("Have ID not Exist In schedule");
        }
        if (ratingPainting.Thirst != null && ratingPainting.Thirst.Except(schedules.Painting.Select(p => p.Id)).ToList().Any()){
            throw new Exception("Have ID not Exist In schedule");
        }
        if (ratingPainting.Fours!= null && ratingPainting.Fours.Except(schedules.Painting.Select(p => p.Id)).ToList().Any()){
            throw new Exception("Have ID not Exist In schedule");
        }

        if (ratingPainting.First != null) ratingPainting.First.Except(schedules.Painting.Select(p => p.Id)).ToList();
        if (ratingPainting.Second != null) ratingPainting.Second.Except(schedules.Painting.Select(p => p.Id)).ToList();
        if (ratingPainting.Thirst != null) ratingPainting.Thirst.Except(schedules.Painting.Select(p => p.Id)).ToList();
        if (ratingPainting.Fours != null) ratingPainting.Fours.Except(schedules.Painting.Select(p => p.Id)).ToList();

        var listPass = schedules.Painting.Where(p => ratingPainting.First != null && ratingPainting.First.Contains(p.Id) &&
                                                            ratingPainting.Second != null && ratingPainting.First.Contains(p.Id) && 
                                                            ratingPainting.Thirst != null && ratingPainting.First.Contains(p.Id) &&
                                                            ratingPainting.Fours != null && ratingPainting.First.Contains(p.Id) &&
                                                            ).ToList();
        var listNotPass = schedules.Painting.Where(p => !ratingPainting.Paintings.Contains(p.Id)).ToList();
        
        listPass.ForEach(p => p.Status = PaintingStatus.Pass.ToString());
        listNotPass.ForEach(p => p.Status = PaintingStatus.NotPass.ToString());
        schedules.Painting.ToList().ForEach(p => p.FinalDecisionTimestamp = DateTime.Now);
        if (listPass.Count != schedules.AwardSchedule.First().Quantity)
        {
            throw new Exception("The Quantity of paiting is wrong");
        }
        
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Update

    public async Task<bool> UpdateSchedule(ScheduleUpdateRequest updateSchedule)
    {
        var schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(updateSchedule.Id);
        if (schedule == null) throw new Exception("Khong tim thay Schedule");
        _mapper.Map(updateSchedule, schedule);
        
        return await _unitOfWork.SaveChangesAsync()>0;
    }

    #endregion

    #region Delete

    public async Task<bool> DeleteSchedule(Guid id)
    {
        var Schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(id);
        if (Schedule == null) throw new Exception("Khong tim thay Schedule"); 
        Schedule.Status = "INACTIVE";

        
        return await _unitOfWork.SaveChangesAsync()>0;
    }

    #endregion
    
    

    
}