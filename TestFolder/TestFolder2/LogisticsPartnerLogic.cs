using B3B4G7.SKS.Package.BusinessLogic.Entities;
using B3B4G7.SKS.Package.BusinessLogic.Interfaces;
using B3B4G7.SKS.Package.DataAccess.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace B3B4G7.SKS.Package.BusinessLogic
{
    public class LogisticsPartnerLogic : ILogisticsPartnerLogic
    {
        private readonly IParcelRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<LogisticsPartnerLogic> _logger;
        public LogisticsPartnerLogic(IMapper mapper, IParcelRepository repo, ILogger<LogisticsPartnerLogic> logger)
        {
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
        }

        public string TransitionParcel(string trackingId, Parcel parcel)
        {
            _logger.LogInformation("Transition Parcel with id={0} to Partner", trackingId);
            DataAccess.Entities.Parcel parcelToDAL = _mapper.Map<DataAccess.Entities.Parcel>(parcel);
            parcelToDAL.TrackingId = trackingId;

            var parcelFromDAL = _repo.Update(parcelToDAL);
            _logger.LogInformation("Parcel id={0} transition completed", trackingId);

            parcel = _mapper.Map<Parcel>(parcelFromDAL);

            return parcel.TrackingId;
        }
    }
}
