document.addEventListener('DOMContentLoaded', () => {
    // Configurar as abas Alugar/Comprar
    const tabAlugar = document.getElementById('tab-alugar');
    const tabComprar = document.getElementById('tab-comprar');

    if (tabAlugar && tabComprar) {
        tabAlugar.addEventListener('click', () => {
            tabAlugar.classList.add('active');
            tabComprar.classList.remove('active');
        });

        tabComprar.addEventListener('click', () => {
            tabComprar.classList.add('active');
            tabAlugar.classList.remove('active');
        });
    }

    // Configurar o formulário de busca
    const searchForm = document.querySelector('.search-form');
    if (searchForm) {
        searchForm.addEventListener('submit', (e) => {
            e.preventDefault();

            const tipo = document.getElementById('input-tipo').value;
            const garagem = document.getElementById('input-garagem').value;
            const dormitorios = document.getElementById('input-dormitorios').value;
            const espaco = document.getElementById('input-espaco').value;

            // Limpar filtros anteriores
            filtros.tipo = [];
            filtros.garagem = [];
            filtros.dormitorios = [];
            filtros.espaco = [];

            // Adicionar os valores selecionados aos filtros
            if (tipo) filtros.tipo.push(tipo);
            if (garagem) filtros.garagem.push(garagem === 'true');
            if (dormitorios) filtros.dormitorios.push(dormitorios);
            if (espaco) filtros.espaco.push(espaco);

            // Aplicar os filtros
            if (typeof aplicarFiltros === 'function') {
                aplicarFiltros();
            }

            // Scroll para os resultados
            const cardsSection = document.getElementById('cards');
            if (cardsSection) {
                cardsSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    }
})