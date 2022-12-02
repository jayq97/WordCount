using AutoMapper;
using B3B4G7.SKS.Package.BusinessLogic.Entities;
using B3B4G7.SKS.Package.BusinessLogic.Interfaces;
using B3B4G7.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3B4G7.SKS.Package.BusinessLogic
{
    public class RecipientLogic : IRecipientLogic
    {
        private readonly IParcelRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<RecipientLogic> _logger;
        public RecipientLogic(IMapper mapper, IParcelRepository repo, ILogger<RecipientLogic> logger)
        {
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
        }

        public Parcel TrackParcel(string trackingId)
        {
            _logger.LogInformation("Tracking Parcel id={0}", trackingId);
            DataAccess.Entities.Parcel parcelFromDAL = _repo.GetByTrackingId(trackingId);

            Parcel parcelToBL = _mapper.Map<Parcel>(parcelFromDAL);

            return parcelToBL;
        }
    }
}
