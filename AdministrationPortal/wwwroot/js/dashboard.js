document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".counter").forEach(counter => {
        const target = +counter.dataset.target;
        let current = 0;

        const step = Math.max(1, Math.floor(target / 80));

        const update = () => {
            current += step;
            if (current >= target) {
                counter.textContent = target;
            } else {
                counter.textContent = current;
                requestAnimationFrame(update);
            }
        };

        update();
    });
});

// =======================
// Manage Users - Search
// =======================
document.addEventListener("DOMContentLoaded", () => {
    const search = document.getElementById("search");

    if (!search) return; // prevents errors on other pages

    search.addEventListener("keyup", e => {
        const q = e.target.value.toLowerCase();

        document.querySelectorAll("tbody tr").forEach(row => {
            row.style.display = row.textContent.toLowerCase().includes(q)
                ? ""
                : "none";
        });
    });
});
