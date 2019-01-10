using System;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Dto
{
    public class FileDto
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        [Required]
        public string FileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [Required]
        public string FileType { get; set; }

        /// <summary>
        /// 文件 Token
        /// </summary>
        [Required]
        public string FileToken { get; set; }

        public FileDto()
        {
            
        }

        public FileDto(string fileName, string fileType)
        {
            FileName = fileName;
            FileType = fileType;
            FileToken = Guid.NewGuid().ToString("N");
        }
    }
}