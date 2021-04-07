
function SelecionarOpInscricao(elemento) {

    var opSelection = elemento.value;
    
    switch (opSelection) {

        case "[Selecione]":
            document.getElementById('divCriarGrupo').style.display = 'none';
       
            break; 
        case "Individual":
            document.getElementById('divCriarGrupo').style.display = 'none';
           document.getElementById('buttonFinalizar').style.display = 'inline';
            break;
        case "Grupo":
            document.getElementById('divCriarGrupo').style.display = 'inline';
            document.getElementById('buttonFinalizar').style.display = 'none';
            break;
    }
}

function CalcularDesconto(elemento, valorCurso) {

    var option = elemento.value;
    var total
    var valorUnit,valor;

    valorUnit = valorCurso;

    switch (option) {

        case 'Selecione':
            document.getElementById('valorTotal').innerHTML = valorCurso;
            break;

        case '2':
            valor = valorUnit * 2;
            total = "R$" + " " + Math.round((valor - (valor * 0.02)));
            document.getElementById("valorTotal").innerHTML = total;
            break;
        case '3':
            valor = valorUnit * 3;
            total = "R$" + " " + Math.round((valor - (valor * 0.03)));
            document.getElementById("valorTotal").innerHTML = total;
            break;
        case '4':
            valor = valorUnit * 4;
            total = "R$" + " " + Math.round((valor - (valor * 0.04)));
            document.getElementById("valorTotal").innerHTML = total;
            break;
        case '5':
            valor = valorUnit * 5;
            total = "R$" + " " + Math.round((valor - (valor * 0.05)));
            document.getElementById("valorTotal").innerHTML = total;
            break;
        case '6':
            valor = valorUnit * 6;
            total = "R$" + " " + Math.round((valor - (valor * 0.06)));
            document.getElementById("valorTotal").innerHTML = total;
            break;
    }
}

function ExibirOpGrupo() {

    document.getElementById('divOpGrupo').style.display = 'flex';
    document.getElementById('divOpGrupo').className = 'form-group row';

    document.getElementById('labOpGrupo').className = 'col-sm-2 col-form-label';
    document.getElementById('selectOpGrupo').className = 'form-control col-sm-2';
}

function ExibirMensagem(mensagem) {

    if (mensagem == 'Pre inscricao realizado com sucesso') {

        alert('Efetue o pagamento para concluir sua inscrição');
        document.getElementById('selectOpInscricao').style.display = 'none';
        document.getElementById('divCriarGrupo').style.display = 'none';
        document.getElementById('buttonFinalizar').style.display = 'inline';
    }
    else if (mensagem == "Grupo ja existe") {
        alert('Já existe um grupo com este nome');
    }
    else if (mensagem == "Senhas nao conferem") {
        alert('Senhas não conferem');
    }
    else if (mensagem == "Vagas indisponiveis") {
        alert('Vagas indisponíveis');
    }
    //else {

    //    ExibirBotaoPagSeguro();
    //}
   
}

