using System.Threading.Tasks;
using Client.Commands;
using Client.ViewModel;

namespace Client.Command;

public class DoubleDownCommand : BaseCommand
{

    private ClientViewModel _viewModel;

    public DoubleDownCommand(ClientViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        _viewModel.HasTurn = false;
        _viewModel.Client.DoubleDown();
    }

    public override Task ExecuteAsync()
    {
        throw new System.NotImplementedException();
    }
}