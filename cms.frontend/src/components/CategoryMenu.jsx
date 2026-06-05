
function CategoryMenu({ categories, onSelect, activeCategory }) {
    return (
        <div className="category-list">
            <button
                className={activeCategory === 0 ? "category-pill active" : "category-pill"}
                onClick={() => onSelect(0)}
            >
                Tất cả
            </button>

            {categories.map((cat) => (
                <button
                    key={cat.id}
                    className={activeCategory === cat.id ? "category-pill active" : "category-pill"}
                    onClick={() => onSelect(cat.id)}
                >
                    {cat.name}
                </button>
            ))}
        </div>
    );
}

export default CategoryMenu;