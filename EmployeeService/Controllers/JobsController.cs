using EmployeeService.Models;
using EmployeeService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost("process")]
        public IActionResult StartJob()
        {
            var job = _jobService.CreateJob();

            return Accepted(new
            {
                jobId = job.JobId,
                status = job.Status,
                message = "Job started successfully"
            });
        }

        [HttpGet("{jobId}")]
        public IActionResult GetJobStatus(string jobId)
        {
            var job = _jobService.GetJob(jobId);

            if (job == null)
            {
                return NotFound(new
                {
                    message = "Job not found"
                });
            }

            return Ok(job);
        }
    }
}