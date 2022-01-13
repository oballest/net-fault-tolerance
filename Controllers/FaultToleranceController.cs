using Microsoft.AspNetCore.Mvc;

namespace faultTolerance.Controllers;

[ApiController]
[Route("[controller]")]
public class FaultToleranceController : ControllerBase
{

     private readonly IHttpClientFactory httpClientFactory;
      private readonly ILogger<FaultToleranceController> _logger;


      public FaultToleranceController(ILogger<FaultToleranceController> logger, IHttpClientFactory _httpClientFactory){

           _logger = logger;
        httpClientFactory = _httpClientFactory;
      }

    [HttpGet("retry")]  
    public async Task<ActionResult<String>> getRetry(){

        var client = httpClientFactory.CreateClient("retry");
        var result = await client.GetStringAsync("/faultTolerance/retry");

        return Ok(result);
    }

    [HttpGet("timeOut")]  
    public async Task<ActionResult<String>> getTimeOut(){

        var client = httpClientFactory.CreateClient("timeOut");
        var result = await client.GetStringAsync("/faultTolerance/timeOut");

        return Ok(result);
    }

    [HttpGet("circuitBreaker")]  
    public async Task<ActionResult<String>> getCircuitBreaker(){

        var client = httpClientFactory.CreateClient("circuitBreaker");
        var result = await client.GetStringAsync("/faultTolerance/circuit");

        return Ok(result);
    }

}