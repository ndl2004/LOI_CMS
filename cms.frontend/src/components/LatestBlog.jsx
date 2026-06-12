import { Link } from "react-router-dom";

function LatestBlog({ posts }) {
    const getImageUrl = (imageUrl) => {
        if (!imageUrl) {
            return "https://via.placeholder.com/600x360?text=LOI+Cosmetics";
        }

        if (imageUrl.startsWith("http")) {
            return imageUrl;
        }

        return `https://localhost:7175${imageUrl}`;
    };

    const getShortContent = (content) => {
        if (!content) return "Cập nhật kiến thức làm đẹp, chăm sóc da và lựa chọn mỹ phẩm phù hợp.";

        return content.length > 90
            ? content.substring(0, 90) + "..."
            : content;
    };

    return (
        <section className="blog-section">
            <div className="blog-head">
                <div>
                    <span className="blog-subtitle">Beauty Blog</span>
                    <h2>Cẩm nang làm đẹp</h2>
                    <p>
                        Chia sẻ kiến thức chăm sóc da, chọn mỹ phẩm và làm đẹp mỗi ngày.
                    </p>
                </div>

                <Link to="/blog" className="blog-view-all">
                    Xem tất cả →
                </Link>
            </div>

            <div className="blog-grid">
                {posts.slice(0, 4).map((post, index) => (
                    <article
                        className={index === 0 ? "blog-card blog-card-large" : "blog-card"}
                        key={post.id}
                    >
                        <div className="blog-image">
                            <img src={getImageUrl(post.imageUrl)} alt={post.title} />

                            <span className="blog-badge">
                                {post.categoryName || "Làm đẹp"}
                            </span>
                        </div>

                        <div className="blog-content">
                            <div className="blog-date">
                                {post.createdDate
                                    ? new Date(post.createdDate).toLocaleDateString("vi-VN")
                                    : "LOI Cosmetics"}
                            </div>

                            <h3>{post.title}</h3>

                            <p>{getShortContent(post.content)}</p>

                            <Link className="blog-read-more" to={`/blog/${post.id}`}>
                                Đọc bài viết →
                            </Link>
                        </div>
                    </article>
                ))}
            </div>
        </section>
    );
}

export default LatestBlog;