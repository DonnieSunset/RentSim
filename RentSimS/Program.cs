using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using RentSimS.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

var configBuiler = new ConfigurationBuilder()
            //.SetBasePath(app.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
var config = configBuiler.Build();
var financeMathServiceURL = config.GetValue<string>("FinanceMathService:url");
var savingPhaseServiceURL = config.GetValue<string>("SavingPhaseService:url");
var rentPhaseServiceURL = config.GetValue<string>("RentPhaseService:url");

//builder.Services.AddHttpClient("SavingPhaseService", x => 
//{
//    x.BaseAddress = new UriBuilder(savingPhaseServiceURL).Uri;
//});
builder.Services.AddSingleton<IFinanceMathClient>(new FinanceMathClient(financeMathServiceURL));
builder.Services.AddSingleton<ISavingPhaseClient>(new SavingPhaseClient(savingPhaseServiceURL));
builder.Services.AddSingleton<IRentPhaseClient>(new RentPhaseClient(rentPhaseServiceURL));



builder.Services
    .AddBlazorise(options =>
    {
        //options.ChangeTextOnKeyPress = true;
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
