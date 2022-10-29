using System.Threading.Tasks;
using Client.Commands;
using Client.ViewModel;

namespace Client.Command;

public class StandCommand : BaseCommand
{

    private ClientViewModel _viewModel;

    public StandCommand(ClientViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        _viewModel.HasTurn = false;
        _viewModel.FirstTurn = false;
        _viewModel.Client.Stand();
    }

    public override Task ExecuteAsync()
    {
    }
}