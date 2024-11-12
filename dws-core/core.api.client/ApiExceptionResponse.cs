using Newtonsoft.Json;

namespace core.api.client
{
	public class ApiExceptionResponse
	{
		[JsonProperty("status")]
		public ApiResposeStatus Status{ get; set; }
		
		[JsonProperty("type")]
		public ApiResponseType Type { get; set; }
		
		[JsonProperty("message")]
		public string Message { get; set; }
		[JsonProperty("data")]
		public object Data { get; set; }
	}
}