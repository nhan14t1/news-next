import { BASE_THUMBNAIL_URL, BASE_URL } from '../shared/constants/app-const';
import { appFetch } from '../shared/utils/apiUtils';

const Sitemap = () => {
  return null;
};

export const getServerSideProps = async ({ res }) => {
  const posts = await appFetch(`/post/site-map`);
  if (!posts || posts.isError) {
    return {
      notFound: true,
    };
  }

  const sitemap = `<?xml version="1.0" encoding="UTF-8"?>
    <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9" xmlns:image="http://www.google.com/schemas/sitemap-image/1.1">
      ${posts
      .map((post) => {
        return `
          <url>
              <loc>${BASE_URL}/article?id=${post.slug}</loc>
              <lastmod>${new Date(post.updatedDate).toISOString()}</lastmod>
              <changefreq>monthly</changefreq>
              <priority>1.0</priority>
              <image:image>
                <image:loc>${BASE_THUMBNAIL_URL}/${post.thumbnailFileName}</image:loc>
                <image:title>${post.title}</image:title>
                <image:caption>${post.introText}</image:caption>
              </image:image>
            </url>
          `;
      })
      .join('')}
    </urlset>
  `;

  res.setHeader('Content-Type', 'text/xml');
  res.write(sitemap);
  res.end();

  return {
    props: {},
  };
};

export default Sitemap;