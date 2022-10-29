using System.Threading.Tasks;
using Client.Commands;
using Client.ViewModel;

namespace Client.Command;

public class HitCommand : BaseCommand
{

    private ClientViewModel _viewModel;

    public HitCommand(ClientViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        _viewModel.FirstTurn = false;
        _viewModel.Client.RequestCard();
    }

    public override Task ExecuteAsync()
    {
    }
}