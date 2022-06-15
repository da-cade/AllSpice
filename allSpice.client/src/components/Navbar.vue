<template>
  <div class="w-100 d-flex justify-content-between bg-light align-items-start">
    <div>
      <img class="headline" src="https://i.imgur.com/uD2hJ1s.png" alt="" />
    </div>
    <div class="myNav bg-dark px-3 d-flex align-items-center">
      <router-link :to="{ name: 'Home' }">
        <div class="hoverable me-3">
          <i class="fa-solid fa-house-chimney"></i>
        </div>
      </router-link>
      <router-link :to="{ name: 'Account' }">
        <div class="hoverable me-3">
          <i class="fa-solid fa-kitchen-set"></i>
        </div>
      </router-link>
      <!-- TODO replace with drawer menu -->
      <div style="position: relative" class="myDrop me-3">
        <button
          class="fa-solid fa-plus"
          type="button"
          id="dropdownMenuButton"
          data-bs-toggle="dropdown"
          data-bs-auto-close="true"
          aria-expanded="false"
        ></button>
        <div id="dropdown-menu" class="dropdown-menu formSize">
          <RecipeForm />
        </div>
      </div>
      <!-- TODO add create form here -->
      <Login />
    </div>
  </div>
</template>

<script>

export default {
  setup() {
    return {
    };
  },
};
</script>

<style scoped>
.headline {
  height: 10vh;
}
.myNav {
  position: fixed;
  width: fit-content;
  height: auto;
  border-radius: 5px;
  right: 0.25rem;
  top: 0.5rem;
}

.dropdown-menu {
  z-index: 100;
}

a {
  color: rgb(255, 236, 91);
}
a:hover {
  text-decoration: none;
  color: rgb(203, 182, 27);
}
svg {
  font-size: 20pt;
}
.formSize {
  height: 30vh;
  width: 55vh;
}
</style>
