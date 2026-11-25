async function showProduct(id) {
    const response = await fetch(`?handler=ProductDetails&id=${id}`);

    if (!response.ok) {
        alert("Could not find product");
        return;
    }

    const html = await response.text();
    const modalBody = document.getElementById("modalBody");

    if (modalBody) {
        modalBody.innerHTML = html;

    }

    const modal = new bootstrap.Modal(document.getElementById("product-modal"));

    modal.show();
}