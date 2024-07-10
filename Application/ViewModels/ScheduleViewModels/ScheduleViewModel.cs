﻿namespace Application.ViewModels.ScheduleViewModels;

public class ScheduleViewModel
{
    public Guid Id { get; set; }
    public Guid? RoundId { get; set; }
    public string? Description { get; set; }
    public Guid? ExaminerId { get; set; }
    public String? Status { get; set; }
}