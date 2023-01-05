namespace Infotecs.Abstractions.Core.Tools;

public interface ICalculator<in TInputData, out TResult>
{
    TResult Calculate(TInputData inputData);
}