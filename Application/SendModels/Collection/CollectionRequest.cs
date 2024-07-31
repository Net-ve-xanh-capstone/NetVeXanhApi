﻿namespace Application.SendModels.Collection;

public class CollectionRequest
{
    public string Name { get; set; }
    public string? Image { get; set; } = null;
    public string Description { get; set; } = "Không có mô tả";

    public Guid CurrentUserId { get; set; }
    public List<Guid> listPaintingId { get; set; }
}