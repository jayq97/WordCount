using AutoMapper;
using B3B4G7.SKS.Package.BusinessLogic.Entities;
using B3B4G7.SKS.Package.BusinessLogic.Interfaces;
using B3B4G7.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3B4G7.SKS.Package.BusinessLogic
{
    public class StaffLogic : IStaffLogic
    {
        private readonly IWarehouseRepository _wRepo;
        private readonly IParcelRepository _pRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<StaffLogic> _logger;
        public StaffLogic(IMapper mapper, IWarehouseRepository wRepo, IParcelRepository pRepo, ILogger<StaffLogic> logger)
        {
            _mapper = mapper;
            _wRepo = wRepo;
            _pRepo = pRepo;
            _logger = logger;
        }

        public void ReportParcelDelivery(string trackingId)
        {
            _logger.LogInformation($"Report parcel delivery from parcel id={trackingId}");
            DataAccess.Entities.Parcel parcel = _pRepo.GetByTrackingId(trackingId);
            parcel.State = DataAccess.Entities.Parcel.StateEnum.DeliveredEnum;
            _logger.LogInformation($"old parcel data {JsonConvert.SerializeObject(parcel)}");
            DataAccess.Entities.Parcel parcelToUpdate = _pRepo.Update(parcel);

            Parcel parcelToBL = _mapper.Map<Parcel>(parcelToUpdate);
        }

        public void ReportParcelHop(string trackingId, string code)
        {
            _logger.LogInformation($"report parcel hop");
            throw new NotImplementedException();
            //_repo.Update(parcelToDAL, arrivedAtStationToDal);

        }
    }
}
