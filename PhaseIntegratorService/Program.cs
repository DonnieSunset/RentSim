using PhaseIntegratorService;
using PhaseIntegratorService.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPhaseIntegrator, PhaseIntegrator>();

var configBuidler = new ConfigurationBuilder()
            //.SetBasePath(app.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
var config = configBuidler.Build();
var financeMathServiceURL = config.GetValue<string>("FinanceMathService:url");
var savingPhaseServiceURL = config.GetValue<string>("SavingPhaseService:url");
var rentPhaseServiceURL = config.GetValue<string>("RentPhaseService:url");

builder.Services.AddSingleton<IFinanceMathClient>(new FinanceMathClient(financeMathServiceURL));
builder.Services.AddScoped<ISavingPhaseClient, SavingPhaseClient>();
builder.Services.AddScoped<IRentPhaseClient, RentPhaseClient>();
builder.Services.AddScoped<IStopWorkPhaseClient, StopWorkPhaseClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();
