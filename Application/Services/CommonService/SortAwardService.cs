using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IService.ICommonService;
using Domain;
using Domain.Models;

namespace Application.Services.CommonService
{
    public  class SortAwardService : ISortAwardService
    {
        private readonly AwardRankPriority _rankPriority;

        public SortAwardService(AwardRankPriority rankPriority) {
            _rankPriority = rankPriority;
        }
        #region
        public List<Award> SortAwards(List<Award> awards)
        {
            return awards.OrderBy(a => _rankPriority.GetRankPriority(a.Rank ?? "")).ToList();
        }
        #endregion
    }
}
