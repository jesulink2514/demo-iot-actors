namespace demo_iot_actors.Infrastructure;

public record ReadSensorValue();
public record RespondSensorValue(decimal Value);
public record AdjustTemperature(decimal TemperatureAdjustment);