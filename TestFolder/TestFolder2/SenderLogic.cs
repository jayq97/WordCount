using AutoMapper;
using B3B4G7.SKS.Package.BusinessLogic.Entities;
using B3B4G7.SKS.Package.BusinessLogic.Interfaces;
using B3B4G7.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace B3B4G7.SKS.Package.BusinessLogic
{
    public class SenderLogic : ISenderLogic
    {
        private readonly IParcelRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<SenderLogic> _logger;
        public SenderLogic(IMapper mapper, IParcelRepository repo, ILogger<SenderLogic> logger)
        {
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
        }

        public string SubmitParcel(Parcel parcel)
        {
            _logger.LogInformation($"Submit Parcel with data {JsonConvert.SerializeObject(parcel)}");
            DataAccess.Entities.Parcel parcelToDAL = _mapper.Map<DataAccess.Entities.Parcel>(parcel);
            var randomizerTextRegex = RandomizerFactory.GetRandomizer(new FieldOptionsTextRegex { Pattern = @"^[A-Z0-9]{9}$" });
            string randomTrackingId = randomizerTextRegex.Generate();
            parcelToDAL.TrackingId = randomTrackingId;

            DataAccess.Entities.Parcel parcelFromDAL = _repo.Create(parcelToDAL);

            Parcel parcelToBL = _mapper.Map<Parcel>(parcelFromDAL);

            
            return parcelToBL.TrackingId;
        }
    }
}

