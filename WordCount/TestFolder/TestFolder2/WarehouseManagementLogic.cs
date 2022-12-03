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
    public class WarehouseManagementLogic : IWarehouseManagementLogic
    {
        private readonly IWarehouseRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<WarehouseManagementLogic> _logger;
        public WarehouseManagementLogic(IMapper mapper, IWarehouseRepository repo, ILogger<WarehouseManagementLogic> logger)
        {
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
        }

        public Warehouse ExportWarehouses()
        {
            return _mapper.Map<BusinessLogic.Entities.Warehouse>(_repo.GetWarehouses());
        }
        public Hop GetWarehouse(string code)
        {
            _logger.LogInformation($"Get warehouse with code{code}");
            DataAccess.Entities.Hop warehouseFromDAL = _repo.GetByCode(code);
            Hop parcelToBL = _mapper.Map<Hop>(warehouseFromDAL);
            _logger.LogInformation($"warehouse fetched from data access layer with data {JsonConvert.SerializeObject(parcelToBL)}");
            return parcelToBL;
        }

        public void ImportWarehouses(Warehouse warehouse)
        {
            _logger.LogInformation($"import warehouse");
            DataAccess.Entities.Warehouse warehouseToDAL = _mapper.Map<DataAccess.Entities.Warehouse>(warehouse);
            _repo.Create(warehouseToDAL);
        }
    }
}
