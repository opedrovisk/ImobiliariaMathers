let imoveis = [];  
async function carregarImoveis() {
    try {
        const resposta = await fetch("http://localhost:3000/imoveis");

        if (!resposta.ok) {
            throw new Error("Erro ao buscar os imóveis");
        }

        imoveis = await resposta.json();

        console.log("IMOVEIS CARREGADOS:", imoveis);

        aplicarFiltros();  
    } catch (error) {
        console.error("Erro ao carregar os imóveis:", error);
    }
}

function gerarCards(imoveisFiltrados) {
    console.log("GERANDO CARDS PARA:", imoveisFiltrados);

    const container = document.querySelector("#cards .row");
    container.innerHTML = "";

    imoveisFiltrados.forEach(imovel => {
        const card = `
            <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                <div class="card h-100">
                    <img src="${imovel.imagem}" class="card-img-top" alt="${imovel.titulo}">
                    <div class="card-body">
                        <h5 class="card-title">${imovel.titulo}</h5>
                        <p class="card-text">
                            Dormitórios: ${imovel.dormitorios} <br>
                            Espaço: ${imovel.espaco}m² <br>
                            Garagem: <strong>${imovel.garagem ? "SIM" : "NÃO"}</strong>
                        </p>
                        <a href="#" class="btn btn-primary">Mais informações</a>
                    </div>
                </div>
            </div>
        `;
        container.innerHTML += card;
    });
}
