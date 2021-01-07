public class InternalServerError {
    public bool? IsError { get; set; }
    public object Data { get; set; }
    public string DeveloperMessage { get; set; }
    public string UserMessage { get; set; }
    public string MoreInfo { get; set; }
    public int? ResponseCode { get; set; }
    public int? HttpStatusCode { get; set; }
    public List<ErrorResponse> Errors { get; set; }
    public string RequestId { get; set; }
}