using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QLNV.Models.DTOs;
using QLNV.Models.Entities;
using QLNV.Repositories;

namespace QLNV.Services
{
    public interface IRequestService
    {
        Task SendRequest(UserRequestDto userRequestDto);
        List<UserRequestDtoGet> GetRequest();
    }
    public class RequestService : IRequestService
    {
        private readonly IMapper _mapper;
        private readonly IUserRequestRepository _userRequestRepository;
        private readonly IWebHostEnvironment _env;

        public RequestService(IMapper mapper, IUserRequestRepository userRequestRepository, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _userRequestRepository = userRequestRepository;
            _env = webHostEnvironment;
        }
        public async Task SendRequest(UserRequestDto userRequestDto)
        {
            if (userRequestDto.Attachment != null)
            {
                //kiểm tra
                var supportedTypes = new[] { "image/jpeg", "image/png", "application/pdf" };
                if (!supportedTypes.Contains(userRequestDto.Attachment.ContentType))
                {
                    throw new Exception("File type is not supported.");
                }
                // tạo tên tệp mới 
                var fileName = Path.GetFileNameWithoutExtension(userRequestDto.Attachment.FileName);
                var extension = Path.GetExtension(userRequestDto.Attachment.FileName);
                var newFileName = Guid.NewGuid().ToString() + extension;

                // đường dẫn thư mục lưu trữ
                var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                var filePath = Path.Combine(uploadFolderPath, newFileName);
                // check kích thước
                long size = userRequestDto.Attachment.Length;
                if (size > (5 * 1024 * 1024))
                {
                    return;
                }
                // lưu tệp vào hệ thống
                using FileStream stream = new FileStream(filePath, FileMode.Create);
                userRequestDto.Attachment.CopyTo(stream);

                // lưu thông tin vào cơ sở dữ liệu
                UserRequest userRequest = new UserRequest
                {
                    UserId = userRequestDto.UserId,
                    Reason = userRequestDto.Reason,
                    AttachmentPath = filePath,
                    AttachmentName = newFileName,
                    AttachmentContentType = userRequestDto.Attachment.ContentType,
                    DayTime = DateTime.Now
                };
                _userRequestRepository.Add(userRequest);

                return;
            }
            throw new Exception("No file upload!!!");
        }
        public List<UserRequestDtoGet> GetRequest()
        {
            List<UserRequest> userRequests = _userRequestRepository.GetAllRequest();
            List<UserRequestDtoGet> userRequestDtoGet = _mapper.Map<List<UserRequestDtoGet>>(userRequests).ToList();
            return userRequestDtoGet;
        }

        //public async Task SendRequest(UserRequestDto userRequestDto)
        //{
        //    if (userRequestDto.Attachment != null)
        //    {
        //        var supportedTypes = new[] { "image/jpeg", "image/png", "application/pdf" };
        //        if (!supportedTypes.Contains(userRequestDto.Attachment.ContentType))
        //        {
        //            throw new Exception("File type is not supported.");
        //        }
        //        // Tạo tên tệp mới để tránh xung đột
        //        var fileName = Path.GetFileNameWithoutExtension(userRequestDto.Attachment.FileName);
        //        var extension = Path.GetExtension(userRequestDto.Attachment.FileName);

        //        var newFileName = $"{fileName}_{DateTime.Now.Ticks}{extension}";
        //        var filePath = Path.Combine(_env.WebRootPath, "Uploads", newFileName);

        //        long size = userRequestDto.Attachment.Length;
        //        if (size > (5 * 1024 * 1024))
        //        {
        //            return;
        //        }

        //        // Lưu tệp vào hệ thống tệp
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await userRequestDto.Attachment.CopyToAsync(stream);
        //        }
        //        //Lưu thông tin vào cơ sở dữ liệu
        //        UserRequest userRequest = new UserRequest
        //        {
        //            UserId = userRequestDto.UserId,
        //            Reason = userRequestDto.Reason,
        //            AttachmentPath = filePath,
        //            AttachmentName = userRequestDto.Attachment.FileName,
        //            AttachmentContentType = userRequestDto.Attachment.ContentType,
        //            DayTime = DateTime.Now
        //        };
        //        _userRequestRepository.Add(userRequest);
        //        //await _userRequestRepository.SaveAsync();

        //        return;
        //    }
        //    throw new Exception("No file upload!!!");
        //}
    }
}
