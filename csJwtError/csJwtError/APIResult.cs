namespace csJwtError;

public class APIResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public ExceptionDetail Exception { get; set; }
}

public class ExceptionDetail
{
    public string Type { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
}
