document.addEventListener("DOMContentLoaded", () => {
    const header = `
        <nav class="navbar navbar-expand-lg bg-body-tertiary px-3 px-md-4 px-lg-5">
            <div class="container-fluid justify-content-between">
                <div style="width: 70px;"></div>
                
                <a class="navbar-brand mx-auto" href="#">
                    <img src="assets/Logo.png" alt="Viviane Imobiliária" class="logo-img">
                </a>
                
                <button class="btn btn-outline-light sobre-btn" data-bs-toggle="modal" data-bs-target="#sobreModal">
                    Sobre
                </button>
            </div>
        </nav>

        <!-- Modal Sobre -->
        <div class="modal fade" id="sobreModal" tabindex="-1" aria-labelledby="sobreModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="sobreModalLabel">Sobre Nós</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                        
                        <p>Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                        
                        <p>Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.getElementById("header").innerHTML = header;
});