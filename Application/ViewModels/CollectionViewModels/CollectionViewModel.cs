﻿namespace Application.ViewModels.CollectionViewModels;

public class CollectionViewModel
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public string Status { get; set; }
    public DateTime UpdatedTime { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
}