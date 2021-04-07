function CriarNovoCurso() {

    var tipoCurso = prompt("Informe o tipo do curso");

    if (tipoCurso == "Online")
        $("form").attr("action", "/ControleProdutos/CreateProdutoOnline");

    else if (tipoCurso == "Presencial") {
        $("form").attr("action", "/ControleProdutos/CreateProdutoPresencial");
    }
        
    else
        alert('Opção inválide');
}