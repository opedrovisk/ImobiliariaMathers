document.addEventListener("DOMContentLoaded", () => {
  const inserirFiltros = `
    <!-- Tratamento imóveis -->
    <div id="resultado-contagem" class="mb-3 text-muted">
      Mostrando 0 imóveis
    </div>
    <div id="nenhum-resultado" class="alert alert-warning d-none">
      Nenhum imóvel encontrado com os filtros selecionados.
    </div>

    <div class="row row-cols-1 row-cols-md-2 row-cols-lg-4 g-3">

      <!-- Tipo -->
      <div class="col">
        <label class="form-label">Tipo:</label>
        <div class="dropdown">
          <button class="btn btn-outline-primary dropdown-toggle w-100" type="button" id="dropdownTipo" data-bs-toggle="dropdown" aria-expanded="false">
            Selecionar Tipo
          </button>
          <ul class="dropdown-menu p-3" aria-labelledby="dropdownTipo">
            <li>
              <div class="form-check">
                <input type="checkbox" id="tipo-casa" value="casa" class="form-check-input">
                <label for="tipo-casa" class="form-check-label">Casa</label>
              </div>
            </li>
            <li>
              <div class="form-check">
                <input type="checkbox" id="tipo-apartamento" value="apartamento" class="form-check-input">
                <label for="tipo-apartamento" class="form-check-label">Apartamento</label>
              </div>
            </li>
          </ul>
        </div>
      </div>

      <!-- Garagem -->
      <div class="col">
        <label class="form-label">Garagem:</label>
        <div class="dropdown">
          <button class="btn btn-outline-primary dropdown-toggle w-100" type="button" id="dropdownGaragem" data-bs-toggle="dropdown" aria-expanded="false">
            Selecionar Garagem
          </button>
          <ul class="dropdown-menu p-3" aria-labelledby="dropdownGaragem">
            <li>
              <div class="form-check">
                <input type="checkbox" id="garagem-sim" value="sim" class="form-check-input">
                <label for="garagem-sim" class="form-check-label">Sim</label>
              </div>
            </li>
            <li>
              <div class="form-check">
                <input type="checkbox" id="garagem-nao" value="nao" class="form-check-input">
                <label for="garagem-nao" class="form-check-label">Não</label>
              </div>
            </li>
          </ul>
        </div>
      </div>

      <!-- Dormitórios -->
      <div class="col">
        <label class="form-label">Dormitórios:</label>
        <div class="dropdown">
          <button class="btn btn-outline-primary dropdown-toggle w-100" type="button" id="dropdownDormitorios" data-bs-toggle="dropdown" aria-expanded="false">
            Selecionar Dormitórios
          </button>
          <ul class="dropdown-menu p-3" aria-labelledby="dropdownDormitorios">
            <li>
              <div class="form-check">
                <input type="checkbox" id="dorm-1" value="1" class="form-check-input">
                <label for="dorm-1" class="form-check-label">1</label>
              </div>
            </li>
            <li>
              <div class="form-check">
                <input type="checkbox" id="dorm-2" value="2" class="form-check-input">
                <label for="dorm-2" class="form-check-label">2</label>
              </div>
            </li>
            <li>
              <div class="form-check">
                <input type="checkbox" id="dorm-3" value="3" class="form-check-input">
                <label for="dorm-3" class="form-check-label">3</label>
              </div>
            </li>
            <li>
              <div class="form-check">
                <input type="checkbox" id="dorm-4" value="4" class="form-check-input">
                <label for="dorm-4" class="form-check-label">4+</label>
              </div>
            </li>
          </ul>
        </div>
      </div>

      <!-- Espaço -->
      <div class="col">
        <label class="form-label">Espaço:</label>
        <div class="dropdown">
          <button class="btn btn-outline-primary dropdown-toggle w-100" type="button" id="dropdownEspaco" data-bs-toggle="dropdown" aria-expanded="false">
            Selecionar Espaço
          </button>
          <ul class="dropdown-menu p-3" aria-labelledby="dropdownEspaco">
            <li>
              <div class="form-check">
                <input type="checkbox" id="espaco-0-50" value="0-50" class="form-check-input">
                <label for="espaco-0-50" class="form-check-label">0-50 m²</label>
              </div>
            </li>
            <li>
              <div class="form-check">
                <input type="checkbox" id="espaco-51-100" value="51-100" class="form-check-input">
                <label for="espaco-51-100" class="form-check-label">51-100 m²</label>
              </div>
            </li>
            <li>
              <div class="form-check">
                <input type="checkbox" id="espaco-101-150" value="101-150" class="form-check-input">
                <label for="espaco-101-150" class="form-check-label">101-150 m²</label>
              </div>
            </li>
            <li>
              <div class="form-check">
                <input type="checkbox" id="espaco-150" value="150+" class="form-check-input">
                <label for="espaco-150" class="form-check-label">150+ m²</label>
              </div>
            </li>
          </ul>
        </div>
      </div>

    </div>

    <!-- Botões -->
    <div class="row mt-3">
      <div class="col text-center">
        <button id="btn-filtrar" class="btn btn-primary me-2">Filtrar</button>
        <button id="btn-limpar" class="btn btn-secondary">Limpar</button>
      </div>
    </div>
  `;

  document.getElementById("filtros").innerHTML = inserirFiltros;
});
