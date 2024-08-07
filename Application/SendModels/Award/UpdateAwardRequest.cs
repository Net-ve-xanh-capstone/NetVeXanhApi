﻿namespace Application.SendModels.Award;

public class UpdateAwardRequest
{
    public Guid Id { get; set; }
    public string Rank { get; set; }
    public int Quantity { get; set; }
    public double? Cash { get; set; } = 0;
    public string? Artifact { get; set; } = "Không có thông tin";
    public Guid UpdatedBy { get; set; }
}