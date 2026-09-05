var builder = DistributedApplication.CreateBuilder(args);

var adminPassword = builder.AddParameter("Password", value: "admin", secret: true);

var postgres = builder
    .AddPostgres("postgres", password: adminPassword, port: 5432)
    .WithImage("timescale/timescaledb-ha")
    .WithImageTag("pg18")
    .WithVolume("ciot_modular_pg_data", "/var/lib/postgresql")
    .WithPgAdmin(x => x.WithHostPort(5050).WithLifetime(ContainerLifetime.Persistent))
    .WithLifetime(ContainerLifetime.Persistent);

var db = postgres.AddDatabase("ciot-db");

var api = builder.AddProject<Projects.CIOT_Api>("ciot-api")
    .WaitFor(db)
    .WithReference(db);

await builder.Build().RunAsync();
