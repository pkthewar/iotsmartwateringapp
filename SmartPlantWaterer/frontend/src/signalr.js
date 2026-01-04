import * as signalR from "@microsoft/signalr";

export function connectToPlant(plantId, onTelemetry) {
  const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5000/hub/telemetry")
    .withAutomaticReconnect()
    .build();

  connection.on("TelemetryUpdate", onTelemetry);

  connection.start().then(() => {
    connection.invoke("JoinPlantGroup", `plant-${plantId}`);
  });

  return connection;
}
