using ClusterBackendAPI.Services.Repo;

namespace ClusterBackendAPI.Services
{
    public abstract class BaseService
    {
        /// <summary>
        /// The repository that will be used to interact with the database.
        /// </summary>
        protected readonly Repository _repository;


        public BaseService(Repository repository)
        {
            _repository = repository;
        }
    }
}
