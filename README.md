

## Galaxy
Next generation framework for Domain Driven Design needs. .Net Core 2.x support !

## IoC
Autofac 

## Adopted principles
SOLID <br/>
Domain Driven Design

## Persistance
EntityFramework Core<br/>
Dapper<br/>
NHibernate

## Event Bus
RabbitMQ support

## Document Databases
MongoDB

## Object Mappers
Mapster<br/>
AutoMapper

## Cache
Redis<br/>
In-Memory

## Object Validation
FluentValidation

## Log
Serilog support

## Modules
Galaxy.Identity

## Advantages

 - Conventinal Dependency Registering
 - Async await first 
 - Multi Tenancy
 - GlobalQuery Filtering
 - Domain Driven Design Concepts
 - Repository and UnitofWork pattern implementations
 - Object to object mapping with abstraction
 - Auto Audit Trailing
 - Domain Events Pattern implementation with MediatR
 - Net Core 2.x support
 - Auto object validation support
 - Aspect Oriented Programming
 - Simple Usage
 
   
## Usages
 
***Basic Usage***

     GalaxyCoreModule.New
                 .RegisterContainerBuilder()
                     .UseGalaxyCore()
                     .UseGalaxyEntityFrameworkCore(
                                new DbContextOptionsBuilder<YourDbContext>()
    				 .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))
                     .InitializeGalaxy();
                         
***MultiTenancy Activation***

    .UseGalaxyEntityFrameworkCore(new DbContextOptionsBuilder<CustomerSampleDbContext>()
	 	.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),typeof(CustomerSampleAppSession))
***Conventional Registration***	 	

     .UseGalaxyCore(b =>
                         {
                             b.UseConventinalCustomRepositories(typeof(BrandRepository).Assembly);
                             b.UseConventinalPolicies(typeof(BrandPolicy).Assembly);
                             b.UseConventinalDomainService(typeof(Brand).Assembly);
                             b.UseConventinalApplicationService(typeof(CustomerAppService).Assembly);
                             b.UseConventinalDomainEvents(typeof(BrandNameChangedDomainEventHandler).Assembly);
                             ...
                         })

***FluentValidators Activation***

    .UseGalaxyFluentValidation(typeof(BrandValidation).Assembly)

     public class BrandValidation : AbstractValidator<BrandDto>
        {
            public BrandValidation()
            {
                RuleFor(t => t.Name).NotEmpty().MinimumLength(3);
                ...
            }
        }

   ***Mapster Activation***

     .UseGalaxyMapster()
***AutoMapper Activation***

	.UseGalaxyAutoMapper()

   ***Interceptors activation***

     .UseGalaxyCore(b =>
                         {
                             b.RegisterAssemblyTypes(typeof(BrandAppService).Assembly)
                                  .AssignableTo<IApplicationService>()
                                  .AsImplementedInterfaces()
                                  .EnableInterfaceInterceptors()
                                  .InterceptedBy(typeof(ValidatorInterceptor))
                                  .InterceptedBy(typeof(UnitOfWorkInterceptor))
                                  .InstancePerLifetimeScope();
                                  ...
                         })

***AggregateRoot definations***

    public class Limit : Entity, IAggregateRoot
        {
	        ...
        }
    public class Brand : FullyAuditEntity, IAggregateRoot
        {
	        ...
        }
        
***Entity definations***

    public class Merchant : Entity, IEntity
        {
	        ...
        }
***Fully Audit Entity definations***

    public  class Brand : FullyAuditEntity , ISoftDelete, IEntity
	    {
			...
	    }

***Soft Delete definations***

    // This interface activate global query filtering on soft delete
    public  class Limit : Entity, ISoftDelete, IAggregateRoot
            {
            ...
            }

***Domain Events definations***

    public class BrandNameChangedDomainEvent : INotification
        {
            public Brand Brand { get; private set; }
            public BrandNameChangedDomainEvent(Brand brand)
            {
                this.Brand = brand;
            }
        }

***Domain Events Firing***

    public Brand ChangeBrandName(string brandName)
      {
         this.BrandName = brandName;
         AddDomainEvent(new BrandNameChangedDomainEvent(this));
         return this;
      }
***Domain Events Handling***

        public sealed class BrandNameChangedDomainEventHandler : INotificationHandler<BrandNameChangedDomainEvent>
            {
                public async Task Handle(BrandNameChangedDomainEvent notification, CancellationToken cancellationToken)
                {
	                ...
                }
            }
***Application Service definations***

    // Query methods all comes from base class. You can override if you want!
     public class BrandAppService : QueryAppServiceAsync<BrandDto, int, Brand>
        {
            public BrandAppService(IRepositoryAsync<Brand, int> repositoryAsync, IObjectMapper objectMapper) : base(repositoryAsync, objectMapper)
            {
            }
            public override Task<BrandDto> FindAsync(int id)
            {
                return base.FindAsync(id);
            }
    
            public override IList<BrandDto> GetAll()
            {
                return base.GetAll();
            }
        }
***Custom Repository definations***
 

    public interface IBrandRepository : ICustomRepository  
     {
     }

***EntityFrameworkCore definations***
   

     public sealed class CustomerSampleDbContext : GalaxyDbContext, IGalaxyContextAsync
        {
            public const string DEFAULT_SCHEMA = "customer";
    
            public CustomerSampleDbContext(DbContextOptions options) : base(options)
            {
            }
    
            public CustomerSampleDbContext(DbContextOptions options, IAppSessionBase appSession) : base(options, appSession)
            {
            }
    
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
                base.OnModelCreating(modelBuilder);
                ...
            }
        }
     public class BrandEntityTypeConfiguration : GalaxyBaseEntityTypeConfigration<Brand>
        {
            public override void Configure(EntityTypeBuilder<Brand> builder)
            {
                base.Configure(builder);
                builder.ToTable("Brands");
		        ...
            }
        }

## Samples
In Samples Repository 
https://github.com/eyazici90/Samples


