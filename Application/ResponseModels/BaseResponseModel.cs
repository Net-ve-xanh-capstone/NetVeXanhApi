﻿namespace Application.ResponseModels;

public class BaseResponseModel
{
    public int Status { get; set; }
    public string Message { get; set; }
    public object Result { get; set; }
}