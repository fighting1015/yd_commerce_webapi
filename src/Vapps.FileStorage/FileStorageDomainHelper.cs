namespace Vapps
{
    public static class FileStorageDomainHelper
    {
        /// <summary>
        /// 获取存储空间域名
        /// </summary>
        /// <returns></returns>
        public static string GetImgBucketDomain()
        {
            FileStorageConsts.BUCKET_DOMAIN.TryGetValue("vapps-img", out string domain);

            return domain;
        }

        /// <summary>
        /// 获取存储空间域名
        /// </summary>
        /// <returns></returns>
        public static string GetStaticBucketDomain()
        {
            FileStorageConsts.BUCKET_DOMAIN.TryGetValue("vapps-static", out string domain);

            return domain;
        }
    }
}
