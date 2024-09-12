using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.S3;

public class S3Options
{
    [Required]
    public string AccessKey { get; set; }

    [Required]
    public string SecretKey { get; set; }

    [Required]
    public string ServiceURL { get; set; }

    [Required]
    public string BucketName { get; set; }
}
