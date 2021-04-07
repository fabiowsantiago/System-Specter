
var table, index;

function InserirModulo() {
    
    var nomeModulo = document.getElementById("NomeModulo");
    table = document.getElementById("tableModulo");
    var qtdLinha = table.rows.length;
    var linha = table.insertRow(qtdLinha);

    var cell = linha.insertCell(0);
    cell.innerHTML = nomeModulo.value;

    PreencheCamposForm();
}

function AlterarModulo() {

    var modulo = document.getElementById("NomeModulo");
    table.rows[index].cells[0].innerHTML = modulo.value;
}


function ExcluirModulo() {

    for (var i = 0; i < table.rows.length; i++) {
        if (index == i) {
            table.deleteRow(index);
            return;
        }
            
    }
}

function PreencheCamposForm() {

    for (var i = 0; i < table.rows.length; i++) {

        table.rows[i].onclick = function ()
        {
            index = this.rowIndex;
            document.getElementById("NomeModulo").value = table.rows[index].cells[0].innerText;
        }
       
    }
}

function SalvarCurso() {

    var valores = []; //declarando uma variável do tipo Array
    var quantidade = table.rows.length;

    if (quantidade > 1) {
        for (var i = 1; i < table.rows.length; i++) { //Pecorrendo a tabela 
            valores.unshift(table.rows[i].cells[0].innerHTML); //Adicionando os valores no Array
        }
    }

    debugger;
   
    var url = "/ControleProdutos/SalvarModulos"; //Declarando a URL da action no controller
   
    $.ajax({
        type: "POST",
        url: url,
        data: {
                modulos: valores,
                nomeCurso: $("#Nome").val()
              }, //Passando os valores por parâmetros para o controller
        success: function (data) {
            alert('Dados enviados com sucesso');
        }
    });
}

function ExibirMensagem(mensagem) {

    if (mensagem == 'Voce esqueceu de informar uma Imagem ou um Video')
        alert('Voce esqueceu de informar uma Imagem ou um Vídeo');

    else if (mensagem = "salvo") {
        alert('Curso Salvo com sucesso');
        document.getElementById("divFormulario").style.display = "none";
        document.getElementById("tabelaModulo").style.display = "inline";
    }
}
       
