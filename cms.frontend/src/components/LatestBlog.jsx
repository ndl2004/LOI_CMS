import { Link } from "react-router-dom";

function LatestBlog({ posts }) {
    const getImageUrl = (imageUrl) => {
        if (!imageUrl) {
            return "https://via.placeholder.com/500x300?text=LOI+Cosmetics";
        }

        if (imageUrl.startsWith("http")) {
            return imageUrl;
        }

        return `https://localhost:7175${imageUrl}`;
    };

    return (
        <section className="section-box">
            <div className="section-head">
                <h2>Cẩm nang làm đẹp</h2>
            </div>

            <div className="product-grid">
                {posts.slice(0, 4).map((post) => (
                    <div className="product-card" key={post.id}>
                        <div className="blog-img">
                            <img src={getImageUrl(post.imageUrl)} alt={post.title} />
                        </div>

                        <div className="product-info">
                            <h3>{post.title}</h3>
                            <p>{post.categoryName}</p>

                            <Link className="btn-detail" to={`/blog/${post.id}`}>
                                Xem bài viết
                            </Link>
                        </div>
                    </div>
                ))}
            </div>
        </section>
    );
}

export default LatestBlog;