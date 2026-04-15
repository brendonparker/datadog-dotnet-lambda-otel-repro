import * as cdk from "aws-cdk-lib";
import { Construct } from "constructs";
import * as lambda from "aws-cdk-lib/aws-lambda";
import { DatadogLambda } from "datadog-cdk-constructs-v2";
import * as path from "path";

export class DatadogReproStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    const ddApiKey = process.env.DD_API_KEY;
    if (!ddApiKey) {
      throw new Error("DD_API_KEY environment variable is required");
    }

    const publishDir = path.join(__dirname, "../../publish/Api");

    const fn = new lambda.Function(this, "ApiFunction", {
      runtime: lambda.Runtime.DOTNET_10,
      handler: "Api",
      code: lambda.Code.fromAsset(publishDir),
      memorySize: 512,
      timeout: cdk.Duration.seconds(30),
      architecture: lambda.Architecture.ARM_64,
      environment: {
        DD_TRACE_OTEL_ENABLED: "true",
      },
    });

    const functionUrl = fn.addFunctionUrl({
      authType: lambda.FunctionUrlAuthType.NONE,
    });

    const datadogLambda = new DatadogLambda(this, "DatadogLambda", {
      dotnetLayerVersion: 23,
      extensionLayerVersion: 94,
      addLayers: true,
      apiKey: ddApiKey,
      service: "dd-repro",
      env: "dev",
    });

    datadogLambda.addLambdaFunctions([fn]);

    new cdk.CfnOutput(this, "FunctionUrl", {
      value: functionUrl.url,
    });
  }
}
