using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Batches.Commands.CreateBatch;
using Quanlyhocthem.Application.Features.Batches.Commands.DeleteBatch;
using Quanlyhocthem.Application.Features.Batches.Commands.UpdateBatch;
using Quanlyhocthem.Application.Features.Batches.Queries.GetBatchById;
using Quanlyhocthem.Application.Features.Batches.Queries.GetBatches;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller quản lý khóa học viên
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class BatchesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BatchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/batches
        // Admin xem danh sách khóa học viên
        [HttpGet]
        public async Task<IActionResult> GetBatches()
        {
            var result = await _mediator.Send(new GetBatchesQuery());

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Lấy danh sách khóa học viên thành công.",
                data = result.Data
            });
        }

        // GET: /api/batches/{batchId}
        // Admin xem chi tiết một khóa học viên
        [HttpGet("{batchId}")]
        public async Task<IActionResult> GetBatchById(Guid batchId)
        {
            var result = await _mediator.Send(new GetBatchByIdQuery
            {
                BatchId = batchId
            });

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Lấy chi tiết khóa học viên thành công.",
                data = result.Data
            });
        }

        // POST: /api/batches
        // Admin tạo khóa học viên
        [HttpPost]
        public async Task<IActionResult> CreateBatch([FromBody] CreateBatchCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Tạo khóa học viên thành công.",
                batchId = result.Data
            });
        }

        // PUT: /api/batches/{batchId}
        // Admin cập nhật khóa học viên
        [HttpPut("{batchId}")]
        public async Task<IActionResult> UpdateBatch(Guid batchId, [FromBody] UpdateBatchCommand command)
        {
            command.BatchId = batchId;

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Cập nhật khóa học viên thành công."
            });
        }

        // DELETE: /api/batches/{batchId}
        // Admin xóa khóa học viên nếu chưa có học viên/lớp học sử dụng
        [HttpDelete("{batchId}")]
        public async Task<IActionResult> DeleteBatch(Guid batchId)
        {
            var result = await _mediator.Send(new DeleteBatchCommand
            {
                BatchId = batchId
            });

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Xóa khóa học viên thành công."
            });
        }
    }
}