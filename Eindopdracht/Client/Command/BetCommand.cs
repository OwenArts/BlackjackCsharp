using System;
using System.Threading.Tasks;
using System.Windows;
using Client.Commands;
using Client.ViewModel;

namespace Client.Command;

public class BetCommand : BaseCommand
{

    private ClientViewModel _viewModel;
    
    public BetCommand(ClientViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        var bet = _viewModel.Bet;
        int betInt;
        try
        {
            betInt = int.Parse(bet);
        }
        catch (Exception)
        {
            MessageBox.Show("Vul een getal in als inzet");
            return;
        }
        _viewModel.Client.Bet(betInt);
    }

    public override Task ExecuteAsync()
    {
        throw new System.NotImplementedException();
    }
}