using JobTracker.Application.Features.Jobs.DTO;
using JobTracker.Domain.Entities;
using Mapster;

namespace JobTracker.Application
{
    public static class ApplicationMapping
    {

        public static void AddApplicationMapping()
        {
            TypeAdapterConfig<Job, JobDTO>.NewConfig()
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.Address, src => new AddressDto
                 {
                     Street = src.Address.Street,
                     City = src.Address.City,
                     State = src.Address.State,
                     ZipCode = src.Address.ZipCode,
                     Latitude = src.Address.Latitude,
                     Longitude = src.Address.Longitude,
                 });
        }
    }
}
