﻿namespace Application.SendModels.Schedule;

public class ScheduleRequest
{
    public string? Description { get; set; }
    public required Guid RoundId { get; set; }
    public DateTime EndDate { get; set; }
    public required List<Guid> ListExaminer { get; set; }
    public Guid CurrentUserId { get; set; }
}