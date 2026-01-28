using Application.Services;
using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//Scoped
builder.Services.AddScoped<IMessageClassifier, RuleBasedMessageClassifier>();
builder.Services.AddScoped<IPedidoAutoService, PedidoAutoService>();
builder.Services.AddScoped<BriefingRespostaService>();
builder.Services.AddScoped<PedidoStatusService>();
builder.Services.AddScoped<PagamentoStatusService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DevCors");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
