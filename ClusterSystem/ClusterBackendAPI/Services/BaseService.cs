using ClusterBackendAPI.Services.Repo;

namespace ClusterBackendAPI.Services
{
    public abstract class BaseService
    {
        /// <summary>
        /// The repository that will be used to interact with the database.
        /// </summary>
        protected readonly Repository _repository;

        protected readonly ILogger<BaseService> _logger;


        public BaseService(Repository repository, ILogger<BaseService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
    }
}
