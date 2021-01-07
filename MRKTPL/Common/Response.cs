public class ErrorResponse : IErrorResponse {

    public string Object { get; set; }
    public string Key { get; set; }
    public object Value { get; set; }
    public bool IsVisible { get; set; } = true;
    public string ErrorMessage { get; set; }

}