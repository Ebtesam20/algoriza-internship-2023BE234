namespace Vezeeta.API.Helpers
{
    public static class DocumentSettings
    {
        public static async Task<string> SaveImageFileAsync(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine("wwwroot/images", fileName); 
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/images/{fileName}";
        }
    }
}
